using System.Text;
using System.Linq;
using Hisoka.Configuration;
using System.Collections.Generic;


namespace Hisoka
{
    class ProjectionQueryParser<T> : IQueryParser<T>
        where T : class
    {
        private readonly IDictionary<string, TokenType> _tokens = new Dictionary<string, TokenType>
        {
            { ",", TokenType.Comma },
            { "(", TokenType.BeginParen },
            { ")", TokenType.EndParen },
            { "[", TokenType.BeginBrack },
            { "]", TokenType.EndBrack }
        };

        public string ParseValues(object[] values)
        {
            if (values == null || values.Length == 0) return string.Empty;

            var rawText = string.Join(",", values);

            if (string.IsNullOrEmpty(rawText)) return string.Empty;

            var root = FieldTree.CreateRootNode();
            var tokens = Tokenize(rawText);
            var fieldTree = CreateFieldTree(tokens, root);
            
            if (fieldTree.Count == 1)
                return fieldTree[0].HasChilds() ? $"new ( {fieldTree[0].ToString()})" : fieldTree[0].ToString();

            return $"new ({string.Join(", ", fieldTree.Select(s => s.ToString()))})";
        }

        private List<FieldTree> CreateFieldTree(Queue<Token> tokens, FieldTree parent)
        {
            List<FieldTree> fields = new List<FieldTree>();
            while (tokens.Count > 0)
            {
                Token token = tokens.Dequeue();
                FieldTree field = new FieldTree(token);

                if (CanAddParent(token))
                {
                    fields.Add(parent);
                    return fields;
                }

                if (field.CanStep(token, parent))
                    continue;

                parent.AddNodes(CreateFieldTree(tokens, field));

                if (parent.IsRootNode())
                    fields.Add(field);
            }

            return fields;
        }

        private static bool CanAddParent(Token token)
        {
            return token.Type == TokenType.Comma ||
                   token.Type == TokenType.EndBrack ||
                   token.Type == TokenType.EndParen;
        }

        private Queue<Token> Tokenize(string rawString)
        {
            Queue<Token> queue = new Queue<Token>();
            StringBuilder builder = new StringBuilder();
            string ch;

            for (var i = 0; i < rawString.Length; i++)
            {
                ch = rawString[i].ToString();
                if (_tokens.ContainsKey(ch))
                {
                    AppendTokenOnQueue(builder, queue);
                    queue.Enqueue(new Token { Value = ch, Type = _tokens[ch] });
                }
                else
                {
                    builder.Append(ch);
                }
            }

            AppendTokenOnQueue(builder, queue);
            return queue;
        }

        private static void AppendTokenOnQueue(StringBuilder builder, Queue<Token> queue)
        {
            if (builder.Length == 0)
                return;
            
            var originalToken = new Token { Value = builder.ToString().Trim().ToLowerInvariant() };

            if (originalToken.Type != TokenType.Property) 
            {
                queue.Enqueue(originalToken);
                return;
            }

            var cacheKey = typeof(T);            
            var property = HisokaConfiguration.GetPropertyMetadataFromCache(cacheKey, originalToken.Value);

            if (property != null) 
            {
                if (!property.AllowProjection) 
                {
                    builder.Clear();
                    return;
                }

                queue.Enqueue(new Token { Value = property.CurrentProperty.Name, Alias = property.Alias });
                builder.Clear(); 
                return;
            }

            var cache = HisokaConfiguration.GetCache();
                
            foreach (var item in cache) 
            {
                if (item.Value.TryGetValue(originalToken.Value, out var metadata)) 
                {
                    property = metadata;
                    break;
                }
            }

            if (property == null) 
            {
                throw new HisokaException($"Property '{originalToken.Value}' not mapped");
            }

            if (!property.AllowProjection) 
            {
                builder.Clear();
                return;
            }

            queue.Enqueue(new Token { Value = property.CurrentProperty.Name, Alias = property.Alias });
            builder.Clear();
        }
    }
}
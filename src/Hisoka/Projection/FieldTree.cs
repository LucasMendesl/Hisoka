using System.Linq;
using System.Collections.Generic;

namespace Hisoka
{
    class FieldTree
    {
        private readonly List<FieldTree> _childFields;

        public FieldType Type { get; private set; }
        public string Name { get; }
        public string Alias { get; }

        public IReadOnlyCollection<FieldTree> Childrens => _childFields.AsReadOnly();

        public FieldTree(Token token)
            : this(FieldType.Object)
        {
            Name = token.Value;
            Alias = token.Alias;
        }

        private FieldTree(FieldType type)
        {
            Type = type;
            _childFields = new List<FieldTree>();
        }

        public static FieldTree CreateRootNode()
        {
            return new FieldTree(FieldType.Root);
        }

        public bool CanStep(Token token, FieldTree parent)
        {
            if (token.Type == TokenType.BeginBrack)
                parent.Type = FieldType.Collection;

            return token.Type == TokenType.BeginBrack || token.Type == TokenType.BeginParen;
        }

        public bool IsRootNode()
        {
            return Type == FieldType.Root;
        }

        public bool HasChilds() => Childrens.Any();

        public void AddNodes(IEnumerable<FieldTree> fields)
        {
            _childFields.AddRange(fields);
        }

        public override string ToString()
        {
            if (!Childrens.Any())
                return $"{Name} AS {Alias}";

            return Type == FieldType.Collection ? $"{Name}.Select(new ({string.Join(", ", Childrens.Select(s => s.ToString()))})) AS {Alias}" : $"new ({string.Join(", ", Childrens.Select(s => s.ToString().StartsWith("new") ? $"{s}" : $"{Name}.{s}"))}) AS {Alias}";
        }
    }
}

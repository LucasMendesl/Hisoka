namespace Hisoka
{
    interface IQueryParser<T>
        where T : class
    {
        string ParseValues(object[] values);
    }
}

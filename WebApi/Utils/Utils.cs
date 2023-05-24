namespace WebApi.Utils;

public static class Query
{
    public static bool BuildQuery<T>(object a, object b)
    {
        List<bool> res = new List<bool>();

        foreach (var property in typeof(T).GetProperties())
        {
            if (property.GetValue(a) != null && property.GetValue(a).GetType() != typeof(string))
            {
                res.Add(property.GetValue(a) == property.GetValue(b));
            }
        }

        return res.All(x => x == true);
    }
}
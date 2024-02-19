using System.Text;

namespace eShop.ServicesProduct.API.Extensions;

public static class StringExtеnsion
{
    public static string[]? CommaSplit(this string str)
    {
        if (str == null)
        {
            return null;
        }

        var length = str.Length;
        StringBuilder stringBuilder = new();
        List<string> strings = new();
        for (int i = 0; i < length; i++)
        {
            if (str[i] != ',' || i + 1 == length)
            {
                stringBuilder.Append(str[i]);
                if (i + 1 != length)
                    continue;
            }
            strings.Add(stringBuilder.ToString());
            stringBuilder = new();
        }

        return strings.ToArray();
    }
}
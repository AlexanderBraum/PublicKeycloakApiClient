using System.IO;
using System.Text;

namespace Keycloak.ApiClient.FluentInterface.Core
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
    }
}

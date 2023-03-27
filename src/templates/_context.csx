#r "Newtonsoft.Json"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

public class ApimContext
{
    public IApi Api { get; set; }
    public IDeployment Deployment { get; set; }
    public TimeSpan Elapsed { get; set; }
    public ILastError LastError { get; set; }
    public IOperation Operation { get; set; }
    public IProduct Product { get; set; }
    public IRequest Request { get; set; }
    public Guid RequestId { get; set; }
    public IResponse Response { get; set; }
    public ISubscription Subscription { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Tracing { get; set; }
    public IUser User { get; set; }
    public Variable Variables { get; set; }

    public ApimContext()
    {
        // Instatiate any objects we need to avoid null reference exceptions
    }

    public void Trace(string message) { }
}

public interface IDeployment
{
    public string GatewayId { get; set; }
    public string Region { get; set; }
    public string ServiceId { get; set; }
    public string ServiceName { get; set; }
    public IDictionary<string, System.Security.Cryptography.X509Certificates.X509Certificate2> Certificates { get; set; }
}

public interface ILastError
{
    public string Source { get; set; }
    public string Reason { get; set; }
    public string Message { get; set; }
    public string scope { get; set; }
    public string Section { get; set; }
    public string Path { get; set; }
    public string PolicyId { get; set; }
}

public interface IOperation
{
    public string Id { get; set; }
    public string Method { get; set; }
    public string Name { get; set; }
    public string UrlTemplate { get; set; }
}

public interface IProduct
{
    public IEnumerable<IApi> Apis { get; set; }
    public bool ApprovalRequired { get; set; }
    public IEnumerable<Group> Groups { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public ProductState State { get; set; }
    public int? SubscriptionLimit { get; set; }
    public bool SubscriptionRequired { get; set; }
    public IWorkspace Workspace { get; set; }
}

public interface IRequest
{
#nullable enable
    public IMessageBody? Body { get; set; }
#nullable disable
    public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get; set; }
    public Header Headers { get; set; }
    public string IpAddress { get; set; }
    public IDictionary<string, string> MatchParameters { get; set; }
    public string Method { get; set; }
    public IUrl OriginalUrl { get; set; }
    public IUrl Url { get; set; }
#nullable enable
    public IPrivateEndpointConnection? PrivateEndpointConnection { get; set; }
#nullable disable
}

public interface IResponse
{
    public IMessageBody Body { get; set; }
    public IDictionary<string, string[]> Headers { get; set; }
    public int StatusCode { get; set; }
    public string StatusReason { get; set; }
}

public interface ISubscription
{
    public DateTime CreatedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string PrimaryKey { get; set; }
    public string SecondaryKey { get; set; }
    public DateTime? StartDate { get; set; }
}

public interface IUser
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public IEnumerable<IGroup> Groups { get; set; }
    public string Id { get; set; }
    public IEnumerable<IUserIdentity> Identities { get; set; }
    public string LastName { get; set; }
    public string Note { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class Variable
{
    public IDictionary<string, object> Values { get; set; }
    public T GetValueOrDefault<T>(string variableName, string defaultValue = null)
    {
        if (Values.TryGetValue(variableName, out var value))
        {
            return (T)value;
        }
        else
        {
            return default(T);
        }
    }
}

public class Header
{
    public IDictionary<string, string> Values { get; set; }
    public string GetValueOrDefault(string headerName, string defaultValue = null)
    {
        if (Values.TryGetValue(headerName, out var value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }
}

public class Query
{
    public IDictionary<string, string> Values { get; set; }
    public string GetValueOrDefault(string queryParameterName, string defaultValue = null)
    {
        if (Values.TryGetValue(queryParameterName, out var value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }
}

public interface IApi
{
    public string Id { get; set; }
    public bool IsCurrentRevision { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Revision { get; set; }
    public IEnumerable<string> Protocols { get; set; }
    public IUrl ServiceUrl { get; set; }
    public string Version { get; set; }
    public ISubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; set; }
    public IWorkspace Workspace { get; set; }
}

public interface IGroup
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public interface IMessageBody
{
    public T As<T>(bool preserveContent = false)
    {
        return default(T);
    }
}

public interface IPrivateEndpointConnection
{
    public string Name { get; set; }
    public string GroupId { get; set; }
    public string MemberName { get; set; }
}

public interface IUrl
{
    public string Host { get; set; }
    public string Path { get; set; }
    public int Port { get; set; }
    public Query Query { get; set; }
    public string QueryString { get; set; }
    public string Scheme { get; set; }
}

public interface ISubscriptionKeyParameterNames
{
    public string Header { get; set; }
    public string Query { get; set; }
}

public interface IUserIdentity
{
    public string Id { get; set; }
    public string Provider { get; set; }
}

public interface IWorkspace
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public enum ProductState
{
    NotPublished,
    Published
}

public class BasicAuthCredentials
{
    public string UserId { get; set; }
    public string Password { get; set; }
}

public class Jwt
{
    public string Algorithm { get; set; }
    public IEnumerable<string> Audiences { get; set; }
    public Claim Claims { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public string Id { get; set; }
    public string Issuer { get; set; }
    public DateTime? IssuedAt { get; set; }
    public DateTime? NotBefore { get; set; }
    public string Subject { get; set; }
    public string Type { get; set; }
}

public class Claim
{
    public IDictionary<string, string> Values { get; set; }
    public string GetValueOrDefault(string claimName, string defaultValue = null)
    {
        if (Values.TryGetValue(claimName, out var value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }
}

public static BasicAuthCredentials AsBasic(this string str)
{
    var parts = str.Split(':');
    return new BasicAuthCredentials { UserId = parts[0], Password = parts[1] };
}

public static bool TryParseBasic(this string str, out BasicAuthCredentials credentials)
{
    try
    {
        credentials = str.AsBasic();
        return true;
    }
    catch
    {
        credentials = null;
        return false;
    }
}

public static string DecodeBase64(this string str)
{
    return Encoding.ASCII.GetString(Convert.FromBase64String(str));
}

public static Jwt AsJwt(this string str)
{
    var parts = str.Split('.');
    var header = parts[0].DecodeBase64();
    var payload = parts[1].DecodeBase64();
    var signature = parts[2].DecodeBase64();

    var headerJson = JObject.Parse(header);
    var payloadJson = JObject.Parse(payload);

    return new Jwt
    {
        Algorithm = headerJson["alg"].Value<string>(),
        Audiences = payloadJson["aud"].Values<string>(),
        //Claims = payloadJson.Properties().ToDictionary(p => p.Name, p => p.Values<string>().ToArray()),
        ExpirationTime = payloadJson["exp"].Value<DateTime?>(),
        Id = payloadJson["jti"].Value<string>(),
        Issuer = payloadJson["iss"].Value<string>(),
        IssuedAt = payloadJson["iat"].Value<DateTime?>(),
        NotBefore = payloadJson["nbf"].Value<DateTime?>(),
        Subject = payloadJson["sub"].Value<string>(),
        Type = payloadJson["typ"].Value<string>()
    };
}

public static bool TryParseJwt(this string str, out Jwt jwt)
{
    try
    {
        jwt = str.AsJwt();
        return true;
    }
    catch
    {
        jwt = null;
        return false;
    }
}

public static byte[] Encrypt(this byte[] data, string alg, byte[] key, byte[] iv)
{
    using (var aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
    }
}

public static byte[] Encrypt(this byte[] data, System.Security.Cryptography.SymmetricAlgorithm alg)
{
    using (var encryptor = alg.CreateEncryptor(alg.Key, alg.IV))
    using (var ms = new MemoryStream())
    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
    {
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }
}

public static byte[] Encrypt(this byte[] data, System.Security.Cryptography.SymmetricAlgorithm alg, byte[] key, byte[] iv)
{
    using (var encryptor = alg.CreateEncryptor(key, iv))
    using (var ms = new MemoryStream())
    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
    {
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }
}

public static byte[] Decrypt(this byte[] data, string alg, byte[] key, byte[] iv)
{
    using (var aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
    }
}

public static byte[] Decrypt(this byte[] data, System.Security.Cryptography.SymmetricAlgorithm alg)
{
    using (var decryptor = alg.CreateDecryptor(alg.Key, alg.IV))
    using (var ms = new MemoryStream())
    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
    {
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }
}

public static byte[] Decrypt(this byte[] data, System.Security.Cryptography.SymmetricAlgorithm alg, byte[] key, byte[] iv)
{
    using (var decryptor = alg.CreateDecryptor(key, iv))
    using (var ms = new MemoryStream())
    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
    {
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }
}

public static bool VerifyNoRevocation(this System.Security.Cryptography.X509Certificates.X509Certificate2 cert)
{
    var chain = new System.Security.Cryptography.X509Certificates.X509Chain();
    chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
    return chain.Build(cert);
}
#r "Newtonsoft.Json"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

public class ApimContext
{
    public Api Api { get; set; }
    public Deployment Deployment { get; set; }
    public TimeSpan Elapsed { get; set; }
    public LastError LastError { get; set; }
    public Operation Operation { get; set; }
    public Product Product { get; set; }
    public Request Request { get; set; }
    public Guid RequestId { get; set; }
    public Response Response { get; set; }
    public Subscription Subscription { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Tracing { get; set; }
    public User User { get; set; }
    public Variable Variables { get; set; }

    public ApimContext(string scriptPath = "")
    {
        string filePath = $"{scriptPath}/context.json";

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var context = JsonConvert.DeserializeObject<ApimContext>(json);
            Api = context.Api;
            Deployment = context.Deployment;
            Elapsed = context.Elapsed;
            LastError = context.LastError;
            Operation = context.Operation;
            Product = context.Product;
            Request = context.Request;
            RequestId = context.RequestId;
            Response = context.Response;
            Subscription = context.Subscription;
            Timestamp = context.Timestamp;
            Tracing = context.Tracing;
            User = context.User;
            Variables = context.Variables;
        }
        else
        {
            Api = new Api();
            Deployment = new Deployment();
            Elapsed = new TimeSpan();
            LastError = new LastError();
            Operation = new Operation();
            Product = new Product();
            Request = new Request();
            RequestId = new Guid();
            Response = new Response();
            Subscription = new Subscription();
            Timestamp = new DateTime();
            Tracing = false;
            User = new User();
            Variables = new Variable();
        }
    }

    public void Trace(string message) { }
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Classes
////////////////////////////////////////////////////////////////////////////////////////////////////

public class Response : IResponse
{
    public MessageBody Body { get; set; }

    public Header Headers { get; set; }

    public int StatusCode { get; set; }

    public string StatusReason { get; set; }

    public Response()
    {
        Body = new MessageBody();
        Headers = new Header();
    }
}

public class User : IUser
{
    public string Email { get; set; }

    public string FirstName { get; set; }

    public IEnumerable<Group> Groups { get; set; }

    public string Id { get; set; }

    public IEnumerable<UserIdentity> Identities { get; set; }

    public string LastName { get; set; }

    public string Note { get; set; }

    public DateTime RegistrationDate { get; set; }

    public User()
    {
        Groups = new List<Group>();
        Identities = new List<UserIdentity>();
    }
}

public class Deployment : IDeployment
{
    public string GatewayId { get; set; }

    public string Region { get; set; }

    public string ServiceId { get; set; }

    public string ServiceName { get; set; }

    public IDictionary<string, System.Security.Cryptography.X509Certificates.X509Certificate2> Certificates { get; set; }

    public Deployment()
    {
        Certificates = new Dictionary<string, System.Security.Cryptography.X509Certificates.X509Certificate2>();
    }
}

public class LastError : ILastError
{
    public string Source { get; set; }

    public string Reason { get; set; }

    public string Message { get; set; }

    public string Scope { get; set; }

    public string Section { get; set; }

    public string Path { get; set; }

    public string PolicyId { get; set; }
}

public class Operation : IOperation
{
    public string Id { get; set; }

    public string Method { get; set; }

    public string Name { get; set; }

    public string UrlTemplate { get; set; }
}

public class Product : IProduct
{
    public IEnumerable<Api> Apis { get; set; }

    public bool ApprovalRequired { get; set; }

    public IEnumerable<Group> Groups { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public ProductState State { get; set; }

    public int? SubscriptionLimit { get; set; }

    public bool SubscriptionRequired { get; set; }

    public Workspace Workspace { get; set; }

    public Product()
    {
        Apis = new List<Api>();
        Groups = new List<Group>();
        Workspace = new Workspace();
    }
}

public class Request : IRequest
{
#nullable enable
    public MessageBody? Body { get; set; }
#nullable disable

    public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get; set; }

    public Header Headers { get; set; }

    public string IpAddress { get; set; }

    public IDictionary<string, string> MatchParameters { get; set; }

    public string Method { get; set; }

    public Url OriginalUrl { get; set; }

    public Url Url { get; set; }

#nullable enable
    public PrivateEndpointConnection? PrivateEndpointConnection { get; set; }
#nullable disable

    public Request()
    {
        Body = new MessageBody();
        Headers = new Header();
        MatchParameters = new Dictionary<string, string>();
        OriginalUrl = new Url();
        Url = new Url();
        PrivateEndpointConnection = new PrivateEndpointConnection();
    }
}

public class Subscription : ISubscription
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

public class Variable : IVariable
{
    public IDictionary<string, object> Values { get; set; }

    public Variable()
    {
        Values = new Dictionary<string, object>();
    }

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

public class Header : IHeader
{
    public IDictionary<string, string> Values { get; set; }

    public Header()
    {
        Values = new Dictionary<string, string>();
    }

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

public class Query : IQuery
{
    public IDictionary<string, string> Values { get; set; }

    public Query()
    {
        Values = new Dictionary<string, string>();
    }

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

public class Api : IApi
{
    public string Id { get; set; }

    public bool IsCurrentRevision { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Revision { get; set; }

    public IEnumerable<string> Protocols { get; set; }

    public Url ServiceUrl { get; set; }

    public string Version { get; set; }

    public SubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; set; }

    public Workspace Workspace { get; set; }

    public Api()
    {
        Protocols = new List<string>();
        ServiceUrl = new Url();
        SubscriptionKeyParameterNames = new SubscriptionKeyParameterNames();
        Workspace = new Workspace();
    }
}

public class Group : IGroup
{
    public string Id { get; set; }

    public string Name { get; set; }
}

public class MessageBody : IMessageBody
{
    public dynamic Content { get; set; }

    public T As<T>(bool preserveContent = false)
    {
        return this.Content.ToObject<T>();
    }
}

public class PrivateEndpointConnection : IPrivateEndpointConnection
{
    public string Name { get; set; }

    public string GroupId { get; set; }

    public string MemberName { get; set; }
}

public class Url : IUrl
{
    public string Host { get; set; }

    public string Path { get; set; }

    public int Port { get; set; }

    public Query Query { get; set; }

    public string QueryString { get; set; }

    public string Scheme { get; set; }

    public Url()
    {
        Query = new Query();
    }
}

public class SubscriptionKeyParameterNames : ISubscriptionKeyParameterNames
{
    public string Header { get; set; }

    public string Query { get; set; }
}

public class UserIdentity : IUserIdentity
{
    public string Id { get; set; }

    public string Provider { get; set; }
}

public class Workspace : IWorkspace
{
    public string Id { get; set; }

    public string Name { get; set; }
}

public class BasicAuthCredentials : IBasicAuthCredentials
{
    public string UserId { get; set; }

    public string Password { get; set; }
}

public class Jwt : IJwt
{
    public string Algorithm { get; set; }

    public List<string> Audiences { get; set; }

    public Claim Claims { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public string Id { get; set; }

    public string Issuer { get; set; }

    public DateTime? IssuedAt { get; set; }

    public DateTime? NotBefore { get; set; }

    public string Subject { get; set; }

    public string Type { get; set; }

    public Jwt()
    {
        Audiences = new List<string>();
        Claims = new Claim();
    }
}

public class Claim : IClaim
{
    public IDictionary<string, string[]> Values { get; set; }

    public Claim()
    {
        Values = new Dictionary<string, string[]>();
    }

    public string[] GetValueOrDefault(string claimName, string[] defaultValue = null)
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

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Interfaces
////////////////////////////////////////////////////////////////////////////////////////////////////

public interface IDeployment
{
    string GatewayId { get; set; }

    string Region { get; set; }

    string ServiceId { get; set; }

    string ServiceName { get; set; }

    IDictionary<string, System.Security.Cryptography.X509Certificates.X509Certificate2> Certificates { get; set; }
}

public interface IResponse
{
    MessageBody Body { get; set; }

    Header Headers { get; set; }

    int StatusCode { get; set; }

    string StatusReason { get; set; }
}

public interface IUser
{
    string Email { get; set; }

    string FirstName { get; set; }

    IEnumerable<Group> Groups { get; set; }

    string Id { get; set; }

    IEnumerable<UserIdentity> Identities { get; set; }

    string LastName { get; set; }

    string Note { get; set; }

    DateTime RegistrationDate { get; set; }
}

public interface ILastError
{
    string Source { get; set; }

    string Reason { get; set; }

    string Message { get; set; }

    string Scope { get; set; }

    string Section { get; set; }

    string Path { get; set; }

    string PolicyId { get; set; }
}

public interface IOperation
{
    string Id { get; set; }

    string Method { get; set; }

    string Name { get; set; }

    string UrlTemplate { get; set; }
}

public interface IProduct
{
    IEnumerable<Api> Apis { get; set; }

    bool ApprovalRequired { get; set; }

    IEnumerable<Group> Groups { get; set; }

    string Id { get; set; }

    string Name { get; set; }

    ProductState State { get; set; }

    int? SubscriptionLimit { get; set; }

    bool SubscriptionRequired { get; set; }

    Workspace Workspace { get; set; }
}

public interface IRequest
{
#nullable enable
    MessageBody? Body { get; set; }
#nullable disable

    System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get; set; }

    Header Headers { get; set; }

    string IpAddress { get; set; }

    IDictionary<string, string> MatchParameters { get; set; }

    string Method { get; set; }

    Url OriginalUrl { get; set; }

    Url Url { get; set; }

#nullable enable
    PrivateEndpointConnection? PrivateEndpointConnection { get; set; }
#nullable disable
}

public interface ISubscription
{
    DateTime CreatedDate { get; set; }

    DateTime? EndDate { get; set; }

    string Id { get; set; }

    string Key { get; set; }

    string Name { get; set; }

    string PrimaryKey { get; set; }

    string SecondaryKey { get; set; }

    DateTime? StartDate { get; set; }
}

public interface IVariable
{
    IDictionary<string, object> Values { get; set; }

    T GetValueOrDefault<T>(string variableName, string defaultValue = null);
}

public interface IHeader
{
    IDictionary<string, string> Values { get; set; }

    string GetValueOrDefault(string headerName, string defaultValue = null);
}

public interface IQuery
{
    IDictionary<string, string> Values { get; set; }

    string GetValueOrDefault(string queryParameterName, string defaultValue = null);
}

public interface IApi
{
    string Id { get; set; }

    bool IsCurrentRevision { get; set; }

    string Name { get; set; }

    string Path { get; set; }

    string Revision { get; set; }

    IEnumerable<string> Protocols { get; set; }

    Url ServiceUrl { get; set; }

    string Version { get; set; }

    SubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; set; }

    Workspace Workspace { get; set; }
}

public interface IGroup
{
    string Id { get; set; }

    string Name { get; set; }
}

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
}

public interface IPrivateEndpointConnection
{
    string Name { get; set; }

    string GroupId { get; set; }

    string MemberName { get; set; }
}

public interface IUrl
{
    string Host { get; set; }

    string Path { get; set; }

    int Port { get; set; }

    Query Query { get; set; }

    string QueryString { get; set; }

    string Scheme { get; set; }
}

public interface ISubscriptionKeyParameterNames
{
    string Header { get; set; }

    string Query { get; set; }
}

public interface IUserIdentity
{
    string Id { get; set; }

    string Provider { get; set; }
}

public interface IWorkspace
{
    string Id { get; set; }

    string Name { get; set; }
}

public interface IBasicAuthCredentials
{
    string UserId { get; set; }

    string Password { get; set; }
}

public interface IJwt
{
    string Algorithm { get; set; }

    List<string> Audiences { get; set; }

    Claim Claims { get; set; }

    DateTime? ExpirationTime { get; set; }

    string Id { get; set; }

    string Issuer { get; set; }

    DateTime? IssuedAt { get; set; }

    DateTime? NotBefore { get; set; }

    string Subject { get; set; }

    string Type { get; set; }
}

public interface IClaim
{
    IDictionary<string, string[]> Values { get; set; }

    string[] GetValueOrDefault(string claimName, string[] defaultValue = null);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Enums
////////////////////////////////////////////////////////////////////////////////////////////////////

public enum ProductState
{
    NotPublished,
    Published
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Extensions
////////////////////////////////////////////////////////////////////////////////////////////////////

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

public static string DecodeBase64Url(this string str)
{
    str = str.Replace('-', '+').Replace('_', '/');
    while (str.Length % 4 != 0)
    {
        str += '=';
    }
    return Encoding.ASCII.GetString(Convert.FromBase64String(str));
}

public static Jwt AsJwt(this string str)
{
    var parts = str.Split('.');
    var header = parts[0].DecodeBase64Url();
    var payload = parts[1].DecodeBase64Url();

    var headerJson = JObject.Parse(header);
    var payloadJson = JObject.Parse(payload);

    Jwt jwt = new Jwt();
    //jwt.Audiences = new List<string>();
    //jwt.Claims = new Claim();

    jwt.Algorithm = headerJson["alg"].Value<string>();
    jwt.Type = headerJson["typ"].Value<string>();
    jwt.Issuer = payloadJson["iss"].Value<string>();
    jwt.Subject = payloadJson["sub"].Value<string>();

    long iat = payloadJson["iat"].Value<long>();
    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(iat);
    jwt.IssuedAt = dateTimeOffset.LocalDateTime;

    long exp = payloadJson["exp"].Value<long>();
    dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(iat);
    jwt.ExpirationTime = dateTimeOffset.LocalDateTime;

    long nbf = payloadJson["nbf"].Value<long>();
    dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(iat);
    jwt.NotBefore = dateTimeOffset.LocalDateTime;

    if(payloadJson["aud"].GetType() == typeof(JArray))
    {
        jwt.Audiences = (List<string>)payloadJson["aud"].Values<string>();
    }
    else
    {
        jwt.Audiences = new List<string>();
        jwt.Audiences.Add(payloadJson["aud"].Value<string>());
    }

    foreach (var item in payloadJson)
    {
        if (item.Key != "iss" && item.Key != "sub" && item.Key != "aud" && item.Key != "exp" && item.Key != "nbf" && item.Key != "iat")
        {
            if(item.Value.GetType() == typeof(JArray))
            {
                jwt.Claims.Values.Add(item.Key, item.Value.Values<string>().ToArray());
            }
            else
            {
                var values = new string[1];
                values[0] = item.Value.Value<string>();
                jwt.Claims.Values.Add(item.Key, values);
            }
        }
    }

    return jwt;
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
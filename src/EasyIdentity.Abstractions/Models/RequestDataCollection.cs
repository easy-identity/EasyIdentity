using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace EasyIdentity.Models;

[DebuggerDisplay("Count = {Count}")]
public partial class RequestDataCollection : IRequestCollection
{
    private readonly Dictionary<string, StringValues> _parameters = [];

    private static readonly IEnumerator<KeyValuePair<string, StringValues>> EmptyIEnumerator = new EmptyEnumerator();

    public static RequestDataCollection Load(string requestPath, IEnumerable<KeyValuePair<string, StringValues>> headers, IEnumerable<KeyValuePair<string, StringValues>> query, IEnumerable<KeyValuePair<string, StringValues>> form, RequestAuthorization? requestAuthorization)
    {
        return new RequestDataCollection(requestPath, headers, query, form, requestAuthorization!);
    }

    public RequestDataCollection(
        string requestPath,
        IEnumerable<KeyValuePair<string, StringValues>> headers,
        IEnumerable<KeyValuePair<string, StringValues>> query,
        IEnumerable<KeyValuePair<string, StringValues>> form,
        RequestAuthorization requestAuthorization)
    {
        if (string.IsNullOrEmpty(requestPath))
        {
            throw new ArgumentException($"'{nameof(requestPath)}' cannot be null or empty.", nameof(requestPath));
        }

        RequestPath = requestPath;
        Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        Query = query ?? throw new ArgumentNullException(nameof(query));
        Form = form ?? throw new ArgumentNullException(nameof(query));
        Authorization = requestAuthorization;
        _parameters = MergeParameters(query, form);
    }

    private static Dictionary<string, StringValues> MergeParameters(IEnumerable<KeyValuePair<string, StringValues>> source1, IEnumerable<KeyValuePair<string, StringValues>> source2)
    {
        var result = new Dictionary<string, StringValues>();

        foreach (var item in source1)
        {
            result[item.Key] = item.Value;
        }

        foreach (var item in source2)
        {
            result[item.Key] = item.Value;
        }

        return result;
    }

    public int Count => _parameters?.Count ?? 0;
    public ICollection<string> Keys => _parameters == null ? Array.Empty<string>() : _parameters.Keys;
    public RequestAuthorization Authorization { get; }
    public IEnumerable<KeyValuePair<string, StringValues>> Headers { get; }
    public IEnumerable<KeyValuePair<string, StringValues>> Query { get; }
    public IEnumerable<KeyValuePair<string, StringValues>> Form { get; }
    public string RequestPath { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public StringValues this[string key]
    {
        get
        {
            if (_parameters == null)
            {
                return StringValues.Empty;
            }

            return TryGetValue(key, out var value) ? value : StringValues.Empty;
        }
    }

    public bool ContainsKey(string key)
    {
        return _parameters?.ContainsKey(key) == true;
    }

    public bool TryGetValue(string key, out StringValues value)
    {
        if (_parameters == null)
        {
            value = default;
            return false;
        }
        return _parameters.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
    {
        if (_parameters == null || _parameters.Count == 0)
        {
            // Non-boxed Enumerator
            return EmptyIEnumerator;
        }
        // Boxed Enumerator
        return _parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (_parameters == null || _parameters.Count == 0)
        {
            // Non-boxed Enumerator
            return EmptyIEnumerator;
        }
        // Boxed Enumerator
        return _parameters.GetEnumerator();
    }

    private class EmptyEnumerator : IEnumerator<KeyValuePair<string, StringValues>>
    {
        private static Dictionary<string, StringValues>.Enumerator s_dictionaryEnumerator = new Dictionary<string, StringValues>.Enumerator();

        public KeyValuePair<string, StringValues> Current => s_dictionaryEnumerator.Current;
        object IEnumerator.Current => s_dictionaryEnumerator.Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return s_dictionaryEnumerator.MoveNext();
        }

        public void Reset()
        {
        }
    }
}

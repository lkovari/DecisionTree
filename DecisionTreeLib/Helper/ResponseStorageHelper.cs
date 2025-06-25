using DecisionTreeLib.Response;

namespace DecisionTreeLib.Helper;

public static class ResponseStorageHelper
{
    private static readonly Dictionary<Type, object> TypedResultMaps = new();

    public static void AddResult<T>(Guid nodeId, IResponse<T> response)
    {
        if (!TypedResultMaps.TryGetValue(typeof(T), out var mapObj))
        {
            mapObj = new ResultMap<T>();
            TypedResultMaps[typeof(T)] = mapObj;
        }

        var map = (ResultMap<T>)mapObj;
        map.ResultMapDictionary[nodeId] = response;
    }

    public static bool TryGetResult<T>(Guid nodeId, out IResponse<T>? response)
    {
        response = default;

        if (TypedResultMaps.TryGetValue(typeof(T), out var mapObj))
        {
            var map = (ResultMap<T>)mapObj;
            return map.ResultMapDictionary.TryGetValue(nodeId, out response);
        }

        return false;
    }

    public static void ClearAll()
    {
        TypedResultMaps.Clear();
    }

    public static void ClearResultsForType<T>()
    {
        TypedResultMaps.Remove(typeof(T));
    }

    public static Dictionary<Guid, IResponse<T>>? GetResultMap<T>()
    {
        if (TypedResultMaps.TryGetValue(typeof(T), out var mapObj))
        {
            var map = (ResultMap<T>)mapObj;
            return map.ResultMapDictionary;
        }

        return null;
    }
    
    private class ResultMap<T>
    {
        public Dictionary<Guid, IResponse<T>> ResultMapDictionary { get; } = new();
    }
}

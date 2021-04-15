mergeInto(LibraryManager.library, {
    LogDebug: function(message) {
        console.log(Pointer_stringify(message));
    },
  
    LogWarning: function(message) {
        console.warn(Pointer_stringify(message));
    },

    LogError: function(message) {
        console.error(Pointer_stringify(message));
    },
    
    GetUrlParameters: function() {
        var str = window.location.search;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    }
});
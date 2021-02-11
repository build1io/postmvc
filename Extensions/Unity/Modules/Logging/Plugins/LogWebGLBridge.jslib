mergeInto(LibraryManager.library, {
    LogDebug: function(message) {
        console.log(Pointer_stringify(message));
    },
  
    LogWarning: function(message) {
        console.warn(Pointer_stringify(message));
    },

    LogError: function(message) {
        console.error(Pointer_stringify(message));
    }
});
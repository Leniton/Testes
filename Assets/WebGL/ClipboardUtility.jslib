var ClipboardUtility = {
    $ClipboardUtility: {},
    
    AddEvents: function (callback) {
        if(typeof Runtime === "undefined") Runtime = { dynCall : dynCall }
        const source = document.querySelector("div.source");

        document.addEventListener('copy', (evt) => {
            var text = "a";
            Runtime.dynCall("vs", callback ,[text])
        });
    },
    CopyToClipboard: function (textPtr) {
        var text = UTF8ToString(textPtr);
        navigator.clipboard.writeText(text).then(function () {
            console.log("Text successfully copied to clipboard");
        }, function (err) {
            console.error("Unable to copy text: ", err);
        });
    },
    ClipboardContainsText: function () {
        if (navigator.clipboard.readText) {
            return 1;
        } else {
            return 0;
        }
    }
};

autoAddDeps(ClipboardUtility, '$ClipboardUtility');
mergeInto(LibraryManager.library, ClipboardUtility);
// Thin wrapper over window.localStorage for cart persistence. Deliberately
// key-agnostic -- the namespaced storage key itself lives in
// CartStorageService.cs, so there's one source of truth for it, not two.
window.cartStorage = {
    getItem: function (key) {
        return window.localStorage.getItem(key);
    },
    setItem: function (key, value) {
        window.localStorage.setItem(key, value);
    },
    removeItem: function (key) {
        window.localStorage.removeItem(key);
    }
};

export function show(message) {
    alert(message);
}

export function action(callback) {
    callback.invokeMethodAsync("Invoke");
}

export function func(callback) {
    callback.invokeMethodAsync("Invoke")
        .then(result => {
            alert(result);
        });
}

import './bootstrap.min.js';

let collapse = {
    getInstance: function (element) {
        if (!element) {
            throw 'element is undefined';
        }

        var instance = new bootstrap.Collapse(document.getElementById(element));
        if (instance) {
            return instance;
        }
        throw 'cannot get collapse instance';
    },
    hide: function (element) {
        let instance = collapse.getInstance(element);
        if (instance) {
            instance.hide();
        }
    },
    show: function (element) {
        let instance = collapse.getInstance(element);
        if (instance) {
            instance.show();
        }
    },
}

export { collapse };
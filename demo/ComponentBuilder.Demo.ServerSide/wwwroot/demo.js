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


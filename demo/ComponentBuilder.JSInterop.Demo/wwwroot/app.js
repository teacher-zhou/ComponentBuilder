export function alert(dotNetReference) {
    if (dotNetReference) {
        dotNetReference.invokeMethodAsync("Invoke");
    }
}
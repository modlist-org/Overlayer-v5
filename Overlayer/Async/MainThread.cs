using System.Collections.Concurrent;
using UnityEngine;

namespace Overlayer.Async;

internal sealed class MainThread : MonoBehaviour {
    private static readonly ConcurrentQueue<Action> queue = new();

    public static void Enqueue(Action action) {
        if(action == null) {
            return;
        }

        queue.Enqueue(action);
    }

    private void Update() {
        while(queue.TryDequeue(out Action action)) {
            try {
                action();
            } catch(Exception ex) {
                Debug.LogException(ex);
            }
        }
    }
}
export class WebSocketSession {
  /** @type string */
  path;
  /** @type WebSocket */
  socket;
  /** @type Map */
  waiting;

  constructor(path) {
    this.path = path;
    this.waiting = new Map();
  }

  open() {
    let protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
    let url = `${protocol}//${window.location.host}${this.path}`;
    this.socket = new WebSocket(url);
    this.socket.addEventListener("message", this.onmessage.bind(this));
    return new Promise((resolve, reject) => {
      this.socket.addEventListener("open", resolve, { once: true });
      this.socket.addEventListener("error", reject, { once: true });
    });
  }

  close() {
    return new Promise((resolve) => {
      this.socket.addEventListener("close", resolve, { once: true });
      this.socket.close();
      this.socket = null;
    });
  }

  /**
   * Call a remote procedure. Return a Promise that is fulfilled when the server responds.
   * @param {string} procedure
   * @param {object} payload
   * @returns @type Promise
   */
  call(procedure, payload) {
    const id = Math.random().toString(16).slice(2);
    return new Promise((resolve, reject) => {
      this.waiting.set(id, (error, responsePayload) => {
        if (error) {
          reject("Server Error: " + error);
        } else {
          resolve(responsePayload);
        }
      });

      this.socket.send(
        JSON.stringify({
          id,
          procedure,
          payload,
        })
      );
    });
  }

  onmessage(event) {
    console.log(`[message] Data received from server: ${event.data}`);
    let response = JSON.parse(event.data);
    let callback = this.waiting.get(response.id);
    this.waiting.delete(response.id);
    callback(response.error, response.payload);
  }
}

export class WebSocketSession {
  /** @type string */
  url = null;
  /** @type WebSocket */
  socket = null;

  constructor(url) {
    this.url = url;
    this.waiting = {};
  }

  open() {
    this.socket = new WebSocket(this.url);
    this.socket.addEventListener("message", this.onmessage.bind(this));
    return new Promise((resolve, reject) => {
      this.socket.addEventListener("open", resolve, { once: true });
      this.socket.addEventListener("error", reject, { once: true });
    });
  }

  close() {
    return new Promise((resolve, reject) => {
      this.socket.addEventListener("close", resolve, { once: true });
      this.socket.close();
    });
  }

  onmessage(event) {
    console.log(`[message] Data received from server: ${event.data}`);
    let response = JSON.parse(event.data);
    let waiting = this.waiting[response.id];
    delete this.waiting[response.id];
    waiting(response.error, response.payload);
  }

  /**
   * Call a remote procedure. Return a Promise that is fulfilled when the server responds.
   * @param {string} procedure
   * @param {object} payload
   * @returns @type Promise
   */
  call(procedure, payload) {
    return new Promise((resolve, reject) => {
      const id = Math.random().toString(16).slice(2);
      this.waiting[id] = (error, responsePayload) => {
        if (error) {
          reject("Server Error: " + error);
        } else {
          resolve(responsePayload);
        }
      };

      this.socket.send(
        JSON.stringify({
          id,
          procedure,
          payload,
        })
      );
    });
  }
}

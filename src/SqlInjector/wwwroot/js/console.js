import db from "./db.js";

class WebSocketSession {
  constructor(socket) {
    socket.onopen = this.connect;
    socket.onmessage = this.consume;
  }

  connect() {
    socket.send(
      JSON.stringify({
        command: "connect",
        payload: database,
      })
    );
  }

  consume(event) {
    console.log(`[message] Data received from server: ${event.data}`);
  }
}

let database = await db.connections.where({ id: 19 }).first();
let socket = new WebSocket(`ws://${window.location.host}/ws`);
let session = new WebSocketSession(socket);

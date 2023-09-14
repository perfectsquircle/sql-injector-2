import { WebSocketSession } from "./WebSocketSession.js";

export class ConsoleSession extends WebSocketSession {
  connect(payload) {
    return this.call("connect", payload);
  }

  query(payload) {
    return this.call("query", payload);
  }
}

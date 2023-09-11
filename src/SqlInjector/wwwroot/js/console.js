import { WebSocketSession } from "./WebSocketSession.js";
import db from "./db.js";

let database = await db.connections.where({ id: 6 }).first();
let session = new WebSocketSession(`wss://${window.location.host}/ws`);
await session.open();
let result = await session.call("connect", database);
let queryResult = await session.call("query", { sql: "select 1;" });

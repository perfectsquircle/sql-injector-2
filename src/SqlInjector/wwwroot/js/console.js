import { WebSocketSession } from "./WebSocketSession.js";
import db from "./db.js";

document.addEventListener("alpine:init", () => {
  Alpine.data("console", () => ({
    async init() {},

    sql: "SELECT 1  as foo, 2 as bar, 3 as baz, pg_sleep(3);",
    executing: false,
    timer: "0ms",

    columns: [],
    rows: [],
    rowCount: "0 results",
    error: null,

    limit: 1000,
    // autoCommit: true,
    // statementsPendingCommit: 0,

    async execute() {
      this.executing = true;
      this.startTimer();
      try {
        let { columns, rows } = await session.call("query", { sql: this.sql });
        this.columns = columns;
        this.rows = rows;
        this.rowCount =
          rows.length == 1 ? "1 result" : `${rows.length} results`;
      } catch (e) {
        this.error = e;
      }
      this.executing = false;
      this.stopTimer();
    },

    startTimer() {
      var timerStart;
      let step = (timestamp) => {
        if (!timerStart) {
          timerStart = timestamp;
        }
        this.timer = Math.floor(timestamp - timerStart) + "ms";
        this.timerAnimationRequest = requestAnimationFrame(step);
      };
      this.timerAnimationRequest = requestAnimationFrame(step);
    },

    stopTimer: function () {
      cancelAnimationFrame(this.timerAnimationRequest);
    },
  }));
});

let database = await db.connections.where({ id: 19 }).first();
let protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
let session = new WebSocketSession(`${protocol}//${window.location.host}/ws`);
await session.open();
await session.call("connect", database);

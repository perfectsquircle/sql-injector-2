import { ConsoleSession } from "./ConsoleSession.js";
import db from "./db.js";
import { getFromQuery } from "./utils.js";

var connectionId = Number(getFromQuery("id"));
let session = new ConsoleSession("/ws/console");

document.addEventListener("alpine:init", () => {
  Alpine.data("console", () => ({
    async init() {
      let database = await db.connections.where({ id: connectionId }).first();
      if (!database) return;
      await session.open();
      await session.connect(database);
    },

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
        let { columns, rows } = await session.query({ sql: this.sql });
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

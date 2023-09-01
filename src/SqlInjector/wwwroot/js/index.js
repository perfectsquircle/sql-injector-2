import db from "./db.js";

document.addEventListener("alpine:init", () => {
  Alpine.data("app", () => ({
    async init() {
      this.connections = await db.connections.toArray();
    },

    connections: [],
  }));
});

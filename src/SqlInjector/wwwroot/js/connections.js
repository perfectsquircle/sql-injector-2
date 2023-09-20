import db from "./db.js";

document.addEventListener("alpine:init", () => {
  Alpine.data("connections", () => ({
    async init() {
      this.connections = await db.connections.orderBy("name").toArray();
    },

    connections: [],
  }));
});

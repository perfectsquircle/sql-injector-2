import db from "/js/db.js";
import { getFromQuery } from "./utils.js";
import colorChoices from "./colorChoices.js";

var connectionId = Number(getFromQuery("id"));

document.addEventListener("alpine:init", () => {
  Alpine.data("create", () => ({
    async init() {
      let connection = await db.connections.where({ id: connectionId }).first();
      if (!connection) return;
      this.connection = connection;
    },

    connection: {},

    colorChoices,

    async handleSubmit(e) {
      await db.connections
        .where({ id: connectionId })
        .modify({ ...this.connection });
      window.location = "/";
    },

    async handleDelete(e) {
      await db.connections.where({ id: connectionId }).delete();
      window.location = "/";
    },
  }));
});

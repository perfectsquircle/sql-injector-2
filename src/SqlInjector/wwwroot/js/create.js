import db from "/js/db.js";
import colorChoices from "./colorChoices";

document.addEventListener("alpine:init", () => {
  Alpine.data("create", () => ({
    connection: {
      name: null,
      host: null,
      port: null,
      database: null,
      username: null,
      password: null,
      color: null,
      databaseType: "PostgreSQL",
    },

    colorChoices,

    async handleSubmit(e) {
      await db.connections.add({ ...this.connection });
      window.location = "/";
    },
  }));
});

import db from "/js/db.js";

document.addEventListener("alpine:init", () => {
  Alpine.data("create", () => ({
    name: null,
    hostname: null,
    port: null,
    database: null,
    username: null,
    password: null,
    color: null,
    databaseType: "PostgreSQL",

    colorChoices: [
      "#FFFFFF",
      "#C7AAFF",
      "#E89F9B",
      "#FFE9B7",
      "#B7E89B",
      "#B1FBFF",
    ],

    async handleSubmit(e) {
      await db.connections.add({
        name: this.name,
        databaseType: this.databaseType,
        host: this.hostname,
        port: this.port,
        database: this.database,
        username: this.username,
        password: this.password,
        color: this.color,
      });

      window.location = "/";
    },
  }));
});

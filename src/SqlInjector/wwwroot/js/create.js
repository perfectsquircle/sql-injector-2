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
    databaseType: 'postgres',

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

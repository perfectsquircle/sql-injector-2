import Dexie from "https://unpkg.com/dexie@3.2.4/dist/modern/dexie.mjs";

document.addEventListener("alpine:init", () => {
  Alpine.data("app", () => ({
    async init() {
      // This code will be executed before Alpine
      // initializes the rest of the component.
      const db = new Dexie("sql_injector");

      db.version(1).stores({
        connections: `id++, host, port, database`,
      });

      // await db.connections.add({
      //   host: "localhost",
      //   port: 5432,
      //   database: "foo_bar",
      // });

      this.connections = await db.connections.toArray();
    },

    connections: [],
  }));
});

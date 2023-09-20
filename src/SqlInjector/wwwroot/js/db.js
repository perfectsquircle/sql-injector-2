import Dexie from "/lib/dexie/dist/modern/dexie.mjs";

const db = new Dexie("sql_injector");
db.version(2).stores({
  connections: `id++, name, host, port, database`,
});

export default db;

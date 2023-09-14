import Dexie from "/lib/dexie/dist/modern/dexie.mjs";

const db = new Dexie("sql_injector");
db.version(1).stores({
  connections: `id++, host, port, database`,
});

export default db;

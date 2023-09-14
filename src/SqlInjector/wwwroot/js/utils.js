export function getFromQuery(variable) {
  let query = new URLSearchParams(window.location.search);
  return query.get(variable);
}

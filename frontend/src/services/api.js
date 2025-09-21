const BASE = import.meta.env.VITE_API_BASE || "http://localhost:5001/api";

export async function apiFetch(path, options = {}) {
  const url = `${BASE}${path.startsWith("/") ? path : "/" + path}`;
  const res = await fetch(url, options);
  if (!res.ok) {
    const text = await res.text().catch(()=>"");
    const err = new Error(res.statusText || "API error");
    err.status = res.status;
    err.body = text;
    throw err;
  }
  return res.json();
}

function createSlug(title: string): string {
  return title
    .trim()
    .toLowerCase()
    .replace(/[^a-zA-Z0-9 ]/g, "")
    .replace(/ +/g, "-");
}

export default createSlug;

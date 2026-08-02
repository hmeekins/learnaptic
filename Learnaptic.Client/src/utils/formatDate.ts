function formatTimeElapsed(datestring: string): string {
  const lastAccessedDate = new Date(datestring);
  const now = new Date();

  const elapsedMilliseconds = now.getTime() - lastAccessedDate.getTime();
  const elapsedDays = Math.floor(elapsedMilliseconds / 1000 / 60 / 60 / 24);

  if (elapsedDays >= 365) {
    const elapsedYears = Math.floor(elapsedDays / 365);

    if (elapsedYears === 1) {
      return `${elapsedYears} year ago`;
    } else {
      return `${elapsedYears} years ago`;
    }
  } else if (elapsedDays >= 30) {
    const elapsedMonths = Math.floor(elapsedDays / 30);

    if (elapsedMonths === 1) {
      return `${elapsedMonths} month ago`;
    } else {
      return `${elapsedMonths} months ago`;
    }
  } else {
    if (elapsedDays === 0) {
      return "Today";
    } else if (elapsedDays === 1) {
      return `Yesterday`;
    } else {
      return `${elapsedDays} days ago`;
    }
  }
}

export default formatTimeElapsed;

export function getLocalTimeZoneOffsetMinutes(): number {
  const jsOffset: number = new Date().getTimezoneOffset();

  // @NOTE invert offset because javascript offset is the wrong way around
  // (it reports how UTC is offset from the local time zone, rather than
  //  the other way around)
  return -jsOffset;
}

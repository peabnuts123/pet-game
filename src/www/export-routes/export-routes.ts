import RouteMap from '@app/route-map';

console.log("Route export proof of concept");
console.log("All routes:");
console.log(RouteMap
  .filter((route) => !route.default)
  .map((route) => route.path));

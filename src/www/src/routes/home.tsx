import { FunctionalComponent, h } from "preact";
import Logger from "@app/util/Logger";

const Home: FunctionalComponent = () => {
  Logger.log("Home route is being rendered");

  return (
    <div class="home">
      <h1>Home</h1>
      <p>This is the Home component.</p>
    </div>
  );
};

export default Home;

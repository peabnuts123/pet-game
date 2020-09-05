import { FunctionalComponent, h } from 'preact';
import classNames from 'classnames';

interface Props {
  inverted?: boolean;
}

const LoadingSpinner: FunctionalComponent<Props> = ({ inverted }: Props) => {
  return (
    <div class={classNames("spinner", { "spinner--inverted": inverted })}>
      <div class="rect rect1"></div>
      <div class="rect rect2"></div>
      <div class="rect rect3"></div>
      <div class="rect rect4"></div>
      <div class="rect rect5"></div>
    </div>
  );
};

export default LoadingSpinner;

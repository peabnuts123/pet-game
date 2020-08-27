import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";
import { observer } from "mobx-react-lite";
import { useState } from "preact/hooks";
import classNames from 'classnames';

import { useServices } from '@app/services';

const Header = observer(() => {
  const [userDropdownVisible, setUserDropdownVisible] = useState<boolean>(false);
  const [mobileMenuVisible, setMobileMenuVisible] = useState<boolean>(false);
  const { UserService } = useServices();

  // Lazy sanity (wake up in the late afternoon)
  if (UserService.isLoggedOut) {
    setUserDropdownVisible(false);
  }

  return (
    <Fragment>
      {mobileMenuVisible && (
        <div className={classNames("header__nav--mobile__background", { 'is-open': mobileMenuVisible })} onClick={() => setMobileMenuVisible(false)} />
      )}

      <header class="header">
        <div class="header__container container">
          <Link class="header__brand" activeClassName="is-active" href="/">
            Pet Game
          </Link>

          <nav class="header__nav--desktop">
            <Link class="header__nav-item--desktop" activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

            {UserService.isLoggedIn ? (
              <a class="header__nav-item--desktop" aria-role="button" onClick={() => setUserDropdownVisible(!userDropdownVisible)}>
                <strong>{UserService.currentUser?.username}</strong> ▾
              </a>
            ) : (
                <Link class="header__nav-item--desktop" activeClassName="is-active" href="/login">Log in</Link>
              )}

            {userDropdownVisible && (
              <div class="header__user-dropdown">
                <Link class="header__user-dropdown__item" activeClassName="is-active" href="/user-profile">
                  User profile
                  </Link>

                <a class="header__user-dropdown__item" aria-role="button" onClick={() => UserService.logOut()}>Log out</a>
              </div>
            )}
          </nav>

          <nav class="header__nav--mobile">
            <div class="header__nav-item--mobile">
              <button class="button button--secondary" onClick={() => setMobileMenuVisible(!mobileMenuVisible)}>≡</button>
            </div>

            <div class={classNames("header__nav--mobile__container", { 'is-open': mobileMenuVisible })}>
              <Link class="header__nav--mobile__item" activeClassName="is-active" href="/">Home</Link>
              <Link class="header__nav--mobile__item" activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

              <div class="header__nav--mobile__item--divider"></div>

              {UserService.isLoggedIn ? (
                <Fragment>
                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/user-profile">
                    🐈 <strong>{UserService.currentUser?.username}</strong>
                  </Link>

                  <a class="header__nav--mobile__item" aria-role="button" onClick={() => UserService.logOut()}>Log out</a>
                </Fragment>
              ) : (
                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/login">
                    Log in
                  </Link>
                )}
            </div>
          </nav>
        </div>
      </header>
    </Fragment>
  );
}) as FunctionalComponent;

export default Header;

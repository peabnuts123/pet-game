import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";
import { observer } from "mobx-react-lite";
import { useState } from "preact/hooks";
import classNames from 'classnames';

import { useStores } from "@app/stores";
import NeedsUserProfile from "@app/components/needs-user-profile";

const Header = observer(() => {
  const [userDropdownVisible, setUserDropdownVisible] = useState<boolean>(false);
  const [mobileMenuVisible, setMobileMenuVisible] = useState<boolean>(false);
  const { UserStore } = useStores();

  // Lazy sanity (wake up in the late afternoon)
  if (UserStore.isUserLoggedOut) {
    setUserDropdownVisible(false);
  }
  // @TODO close mobile menu if window goes to tablet breakpoint

  const hideMenus = (): void => {
    setUserDropdownVisible(false);
    setMobileMenuVisible(false);
  };

  return (
    <Fragment>
      {(mobileMenuVisible) && (
        <div className={classNames("header__nav--mobile__background", { 'is-open': mobileMenuVisible })} onClick={() => hideMenus()} />
      )}
      {(userDropdownVisible) && (
        <div className={classNames("header__nav--desktop__background")} onClick={() => hideMenus()} />
      )}

      <header class="header">
        <div class="header__container container">
          <Link class="header__brand" activeClassName="is-active" href="/">
            Pet Game
          </Link>

          <nav class="header__nav--desktop">
            <Link class="header__nav-item--desktop" activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

            {UserStore.isUserLoggedIn ? (
              <a class="header__nav-item--desktop" onClick={() => setUserDropdownVisible(!userDropdownVisible)}>
                <strong>{UserStore.currentUserProfile!.username}</strong> ▾
              </a>
            ) : (
              <div class="header__nav-item--desktop">
                <NeedsUserProfile inverted={true}>
                  <Link activeClassName="is-active" href="/login">Log in / Register</Link>
                </NeedsUserProfile>
              </div>
              )}

            {userDropdownVisible && (
              <div class="header__user-dropdown">
                <Link class="header__user-dropdown__item" activeClassName="is-active" href="/user-profile">
                  User profile
                </Link>

                <Link class="header__user-dropdown__item" activeClassName="is-active" href="/logout">Log out</Link>
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

              {UserStore.isUserLoggedIn ? (
                <Fragment>
                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/user-profile">
                    🐈 <strong>{UserStore.currentUserProfile!.username}</strong>
                  </Link>

                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/logout">Log out</Link>
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

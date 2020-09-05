import { Fragment, FunctionalComponent, h } from "preact";
import { Link } from "preact-router/match";
import { observer } from "mobx-react-lite";
import { useState } from "preact/hooks";
import classNames from 'classnames';
import {
  ChevronDown as ChevronDownIcon,
  ChevronUp as ChevronUpIcon,
  LogIn as LogInIcon,
  LogOut as LogOutIcon,
  User as UserIcon,
  Menu as MenuIcon,
} from 'react-feather';

import { useStores } from "@app/stores";
import LoadingSpinner from "./loading-spinner";
import useRouteChange from "@app/hooks/use-route-change";

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

  // Close menus on route change (i.e. when you navigate using on the of the menus)
  useRouteChange((_args) => {
    hideMenus();
  });

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

          {/* DESKTOP NAV */}
          <nav class="header__nav--desktop">
            {/* Links */}
            <Link class="header__nav-item--desktop" activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

            {/* Loading spinner */}
            {UserStore.isFetchingUserProfile && (
              <div class="header__nav-item--desktop">
                <LoadingSpinner inverted={true} />
              </div>
            )}

            {/* Profile button */}
            {UserStore.isUserLoggedIn && (
              /* Profile */
              <a class="header__nav-item--desktop" onClick={() => setUserDropdownVisible(!userDropdownVisible)}>
                <strong>{UserStore.currentUserProfile!.username}</strong>
                {userDropdownVisible ? (
                  <ChevronUpIcon />
                ) : (
                    <ChevronDownIcon size={20} />
                  )}
              </a>
            )}

            {/* Log in button */}
            {UserStore.isUserLoggedOut && (
              <Link class="header__nav-item--desktop" activeClassName="is-active" href="/login"><LogInIcon className="u-margin-right-md" /> Log in / Register</Link>
            )}

            {/* Desktop dropdown */}
            {userDropdownVisible && (
              <div class="header__user-dropdown">
                <Link class="header__user-dropdown__item" activeClassName="is-active" href="/user-profile">
                  <UserIcon className="u-margin-right-md" /> User profile
                </Link>

                <Link class="header__user-dropdown__item" activeClassName="is-active" href="/logout">
                  <LogOutIcon className="u-margin-right-md" /> Log out
                </Link>
              </div>
            )}
          </nav>

          {/* MOBILE NAV */}
          <nav class="header__nav--mobile">
            {/* Menu toggle */}
            <div class="header__nav-item--mobile">
              <button class="button button--secondary" onClick={() => setMobileMenuVisible(!mobileMenuVisible)}><MenuIcon size={20} /></button>
            </div>

            {/* Mobile menu */}
            <div class={classNames("header__nav--mobile__container", { 'is-open': mobileMenuVisible })}>
              {/* Links */}
              <Link class="header__nav--mobile__item" activeClassName="is-active" href="/">Home</Link>
              <Link class="header__nav--mobile__item" activeClassName="is-active" href="/taking-tree">The Taking Tree</Link>

              {/* Divider */}
              <div class="header__nav--mobile__item--divider"></div>

              {/* Loading spinner */}
              {UserStore.isFetchingUserProfile && (
                <div class="header__nav--mobile__item">
                  <LoadingSpinner inverted={true} />
                </div>
              )}

              {/* Profile / log-out */}
              {UserStore.isUserLoggedIn && (
                <Fragment>
                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/user-profile">
                    <UserIcon className="u-margin-right-md" /> <strong>{UserStore.currentUserProfile!.username}</strong>
                  </Link>

                  <Link class="header__nav--mobile__item" activeClassName="is-active" href="/logout">
                    <LogOutIcon className="u-margin-right-md" /> Log out
                  </Link>
                </Fragment>
              )}

              {/* Log-in */}
              {UserStore.isUserLoggedOut && (
                <Link class="header__nav--mobile__item" activeClassName="is-active" href="/login">
                  <LogInIcon className="u-margin-right-md" /> Log in
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

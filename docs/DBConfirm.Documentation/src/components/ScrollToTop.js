import { useEffect } from "react";
import { useLocation } from "react-router-dom";

export default function ScrollToTop({scrollContent}) {
  const { pathname } = useLocation();

  useEffect(() => {
    scrollContent.current.scrollTop = 0;
  }, [pathname, scrollContent]);

  return null;
}
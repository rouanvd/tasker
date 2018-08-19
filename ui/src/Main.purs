module Main where

import Prelude
import Effect (Effect)
import Effect.Console (log)
import Data.Maybe (fromJust)
import Foreign.Object (Object, empty, insert)
import Partial.Unsafe (unsafePartial)
import Webfu.DOM (doc_querySelector)
import Webfu.Mithril (Component, route) as M

main :: Effect Unit
main = do
  body <- unsafePartial $ fromJust <$> doc_querySelector( "body" )
  M.route body "/Welcome" routes
  pure unit
  where
    routes :: Object M.Component
    routes = empty -- # insert "/Welcome" (DisplayScreenView.mkView (mkDisplayScreenPresenter {indicators:[]}))

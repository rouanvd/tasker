module Webfu.Mithril (
  VNode,
  Component,
  mkVNode,
  mkTextVNode,
  mkComponentVNode,
  mkComponent,
  onInit,
  onCreate,
  onBeforeUpdate,
  onUpdate,
  onBeforeRemove,
  onRemove,
  raise,
  mount,
  route,

  parseQueryString,
  buildQueryString,
  redraw,
  trust,
  version
) where

import Prelude (class Show, Unit, unit, ($))
import Data.Function.Uncurried (Fn1, runFn1, Fn3, runFn3, Fn4, runFn4, Fn5, runFn5, Fn6, runFn6)
import Data.Maybe
import Data.Either (Either(..), fromLeft, fromRight, isLeft)
import Foreign.Object (Object)
import Partial.Unsafe (unsafePartial)
import Effect (Effect)
import Effect.Ref (Ref)
import Effect.Ref (write, read, new) as RefM
import Webfu.Data.ObjMap (Obj)
import Webfu.DOM (Element)

type Attributes = Obj


---------------------------------------------------------------
-- VNODE
---------------------------------------------------------------

foreign import data VNode :: Type

foreign import vnodeTag :: VNode -> String

foreign import vnodeKey_foreign :: forall a. Maybe a -> (a -> Maybe a) -> VNode -> Maybe String
vnodeKey :: VNode -> Maybe String
vnodeKey vnode = vnodeKey_foreign Nothing Just vnode

foreign import vnodeChildren_foreign :: forall a. Maybe a -> (a -> Maybe a) -> VNode -> Maybe (Array VNode)
vnodeChildren :: VNode -> Maybe (Array VNode)
vnodeChildren vnode = vnodeChildren_foreign Nothing Just vnode


instance vnodeShow :: Show VNode where
  show vnode = "VNode"


foreign import mkVNode_foreign :: forall r. Fn6
                                  (Either String Element -> Boolean)
                                  (Either String Element -> String)
                                  (Either String Element -> Element)
                                  (Either String Element)
                                  Attributes
                                  (Either String (Array VNode))
                                  VNode

mkVNode :: forall r. (Either String Element) -> Attributes -> Array VNode -> VNode
mkVNode selector attrs childNodes = runFn6 (mkVNode_foreign) isLeft (unsafePartial $ fromLeft) (unsafePartial $ fromRight) selector attrs (Right childNodes)

mkTextVNode :: forall r. (Either String Element) -> Attributes -> String -> VNode
mkTextVNode selector attrs childNodes = runFn6 (mkVNode_foreign) isLeft (unsafePartial $ fromLeft) (unsafePartial $ fromRight) selector attrs (Left childNodes)


---------------------------------------------------------------
-- COMPONENT
---------------------------------------------------------------

foreign import data Component :: Type


foreign import mkComponent_foreign :: forall s. Fn5
                                      (s -> Effect (Ref s))
                                      (Ref s -> Effect s)
                                      (s -> Ref s -> Effect Unit)
                                      s
                                      (Ref s -> VNode -> Effect VNode)
                                      Component

-- | Creates a new Component which uses the supplied view function.
mkComponent :: forall s. s -> (Ref s -> VNode -> Effect VNode) -> Component
mkComponent state viewF = runFn5 (mkComponent_foreign) (RefM.new) (RefM.read) (RefM.write) state viewF


foreign import mkComponentVNode_foreign :: Fn1 Component VNode
mkComponentVNode :: Component -> VNode
mkComponentVNode component = runFn1 mkComponentVNode_foreign component


foreign import onInit_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onInit :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onInit initF component = runFn4 onInit_foreign unit RefM.read component initF


foreign import onCreate_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onCreate :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onCreate createF component = runFn4 onCreate_foreign unit RefM.read component createF


foreign import onBeforeUpdate_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onBeforeUpdate :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onBeforeUpdate beforeUpdateF component = runFn4 onUpdate_foreign unit RefM.read component beforeUpdateF


foreign import onUpdate_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onUpdate :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onUpdate updateF component = runFn4 onUpdate_foreign unit RefM.read component updateF


foreign import onBeforeRemove_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onBeforeRemove :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onBeforeRemove beforeRemoveF component = runFn4 onBeforeRemove_foreign unit RefM.read component beforeRemoveF


foreign import onRemove_foreign :: forall s. Fn4 Unit (Ref s -> Effect s) Component (Ref s -> s -> VNode -> Effect Unit) Component
onRemove :: forall s. (Ref s -> s -> VNode -> Effect Unit) -> Component -> Component
onRemove removeF component = runFn4 onRemove_foreign unit RefM.read component removeF


---------------------------------------------------------------
-- EVENTS
---------------------------------------------------------------

foreign import raise_foreign :: forall m s. Fn6
                                Unit
                                (Ref s -> Effect s)
                                (s -> Ref s -> Effect Unit)
                                (Ref s -> m -> Effect Unit)
                                (Ref s)
                                m
                                Unit

raise :: forall m s. (Ref s -> m -> Effect Unit) -> Ref s -> m -> Unit
raise updateF state msg = runFn6 (raise_foreign) unit RefM.read RefM.write updateF state msg


---------------------------------------------------------------
-- MITHRIL CORE
---------------------------------------------------------------

foreign import render_foreign :: Unit
                              -> (Either String (Array VNode) -> Boolean)
                              -> (Either String (Array VNode) -> String)
                              -> (Either String (Array VNode) -> Array VNode)
                              -> Element
                              -> Either String (Array VNode)
                              -> Effect Unit

-- | Renders a text node to the DOM
renderText :: Element -> String -> Effect Unit
renderText elem vnodes = render_foreign unit isLeft (unsafePartial $ fromLeft) (unsafePartial $ fromRight) elem (Left vnodes)

-- | Renders 1 or more VNodes to the DOM
render :: Element -> Array VNode -> Effect Unit
render elem vnodes = render_foreign unit isLeft (unsafePartial $ fromLeft) (unsafePartial $ fromRight) elem (Right vnodes)

foreign import mount_foreign :: Fn3 Unit Element Component (Effect Unit)
mount :: Element -> Component -> Effect Unit
mount elem component = runFn3 mount_foreign unit elem component

-- foreign import unmount :: forall eff. Element -> Eff (dom :: DOM | eff) Unit

foreign import route_foreign :: Fn4 Unit Element String (Object Component) (Effect Unit)
-- | Configures the navigation for your Mithril application.
-- | You can only call `route` once per application.
route :: Element -> String -> Object Component -> Effect Unit
route rootElem defaultRoute routes = runFn4 (route_foreign) unit rootElem defaultRoute routes


-- buildQueryString :: forall a. Generic a => a -> String
-- buildQueryString r =
--   let typRep = toSpine r in
--   case typRep of
--     SProd name [ctorF] ->
--       case ctorF unit of
--         SRecord fields -> foldl (\ acc f -> acc <> (if length acc > 0 then "," else "") <> f.recLabel <> "=" <> (show $ unsafePartial <<< fromJust <<< fromSpine $ f.recValue unit)) "" fields
--         _              -> "bluh"
--     _                 -> "bluh"
--     -- where
    --   recValueAsStr ::

-- | Turns a string of the form ?a=1&b=2 into an object
foreign import parseQueryString :: forall r. String -> {|r}

-- | Turns an object into a string of form a=1&b=2
foreign import buildQueryString :: forall r. {|r} -> String

foreign import redraw_foreign :: Unit -> Effect Unit
-- | Updates the DOM after a change in the application data layer.
redraw :: Effect Unit
redraw = redraw_foreign unit

-- | Turns an HTML string into unescaped HTML.
foreign import trust :: String -> VNode

-- | The semver version number of the current Mithril library.
foreign import version :: String

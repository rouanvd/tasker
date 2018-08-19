module Webfu.DOM.Events
( Event
, KeyboardEvent
, MouseEvent
, type_
, bubbles
, cancelable
, defaultPrevented
, preventDefault
) where

import Prelude (Unit, unit, (<<<))
import Effect (Effect)
import Unsafe.Coerce (unsafeCoerce)
import Webfu.Interop
import Webfu.Data.Cast (class Cast)


foreign import data Event :: Type


type_ :: forall a. Cast a Event => a -> String
type_ = (readString "type") <<< toJsObject

bubbles :: forall a. Cast a Event => a -> String
bubbles = (readString "bubbles") <<< toJsObject

cancelable :: forall a. Cast a Event => a -> String
cancelable = (readString "cancelable") <<< toJsObject

defaultPrevented :: forall a. Cast a Event => a -> Boolean
defaultPrevented = (readBoolean "defaultPrevented") <<< toJsObject

preventDefault :: forall a. Cast a Event => a -> Effect Unit
preventDefault = (runEffMethod0 "preventDefault") <<< toJsObject


-----------------------------------------------------------
-- KeyboardEvent
-----------------------------------------------------------

foreign import data KeyboardEvent :: Type


instance castKeyboardEventToEvent :: Cast KeyboardEvent Event where
  cast = unsafeCoerce


-----------------------------------------------------------
-- MouseEvent
-----------------------------------------------------------

foreign import data MouseEvent :: Type


instance castMouseEventToEvent :: Cast MouseEvent Event where
  cast = unsafeCoerce

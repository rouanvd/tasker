module Webfu.DOM (
  module Webfu.DOM.Core,
  nodeChildNodes, nodeFirstChild, nodeLastChild, nodeNextSibling, nodePreviousSibling, nodeName, nodeType, nodeValue, nodeOwnerDocument, nodeParentNode, nodeParentElement,
  window,
  win_alert,
  document,
  doc_getElementById,
  doc_querySelector,
  elem_id,
  elem_attr,
  el_setAttr,
  el_prop_value,
  el_prop_setValue,
  el_setOnMouseMove
) where

import Prelude (Unit, unit, ($))
import Data.Maybe
import Data.Function.Uncurried (Fn3, runFn3)
import Effect (Effect)
import Unsafe.Coerce (unsafeCoerce)
import Webfu.Interop
import Webfu.DOM.Core
import Webfu.DOM.Events
import Webfu.DOM.Promise (Promise)


---------------------------------------------------------------
-- NODE
---------------------------------------------------------------

foreign import nodeChildNodes :: Node -> Array Node

foreign import nodeFirstChild_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Node
nodeFirstChild :: Node -> Maybe Node
nodeFirstChild n = nodeFirstChild_foreign Nothing Just n

foreign import nodeLastChild_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Node
nodeLastChild :: Node -> Maybe Node
nodeLastChild n = nodeLastChild_foreign Nothing Just n

foreign import nodeNextSibling_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Node
nodeNextSibling :: Node -> Maybe Node
nodeNextSibling n = nodeNextSibling_foreign Nothing Just n

foreign import nodePreviousSibling_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Node
nodePreviousSibling :: Node -> Maybe Node
nodePreviousSibling n = nodePreviousSibling_foreign Nothing Just n

foreign import nodeName :: Node -> String

foreign import nodeType :: Node -> Int

foreign import nodeValue_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe String
nodeValue :: Node -> Maybe String
nodeValue n = nodeValue_foreign Nothing Just n

foreign import nodeOwnerDocument_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Document
nodeOwnerDocument :: Node -> Maybe Document
nodeOwnerDocument n = nodeOwnerDocument_foreign Nothing Just n

foreign import nodeParentNode_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Node
nodeParentNode :: Node -> Maybe Node
nodeParentNode n = nodeParentNode_foreign Nothing Just n

foreign import nodeParentElement_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Node -> Maybe Element
nodeParentElement :: Node -> Maybe Element
nodeParentElement n = nodeParentElement_foreign Nothing Just n


---------------------------------------------------------------
-- WINDOW
---------------------------------------------------------------

foreign import window :: Effect Window


foreign import win_alert_foreign :: Unit -> String -> Window -> Effect Unit
win_alert :: String -> Window -> Effect Unit
win_alert msg w = win_alert_foreign unit msg w


---------------------------------------------------------------
-- DOCUMENT
---------------------------------------------------------------

foreign import document :: Effect Document

foreign import doc_getElementById_foreign :: forall a. Maybe a -> (a -> Maybe a) -> String -> Effect (Maybe Element)
doc_getElementById :: String -> Effect (Maybe Element)
doc_getElementById id = doc_getElementById_foreign Nothing Just id

foreign import doc_querySelector_foreign :: forall a. Maybe a -> (a -> Maybe a) -> String -> Effect (Maybe Element)
doc_querySelector :: String -> Effect (Maybe Element)
doc_querySelector id = doc_querySelector_foreign Nothing Just id


---------------------------------------------------------------
-- ELEMENT
---------------------------------------------------------------

foreign import elemId_foreign :: forall a. Maybe a -> (a -> Maybe a) -> Element -> Maybe String
elem_id :: Element -> Maybe String
elem_id e = elemId_foreign Nothing Just e


foreign import elem_attr_foreign :: forall a. Maybe a -> (a -> Maybe a) -> String -> Element -> Maybe String
elem_attr :: String -> Element -> Maybe String
elem_attr attrName e  = elem_attr_foreign Nothing Just attrName e


foreign import el_setAttr_foreign :: Unit -> String -> String -> Element -> Effect Unit
el_setAttr :: String -> String -> Element -> Effect Unit
el_setAttr attrName attrVal e  = el_setAttr_foreign unit attrName attrVal e


foreign import elem_prop_foreign :: forall a. Maybe a -> (a -> Maybe a) -> String -> Element -> Maybe a
el_prop_value :: Element -> Maybe String
el_prop_value e =
  let
    maybePropVal = elem_prop_foreign Nothing Just "value" e
  in
    case maybePropVal of
      Nothing -> Nothing
      Just v  -> Just $ unsafeCoerce v

el_prop_setValue :: Element -> Maybe String -> Effect Unit
el_prop_setValue e value =
  setString' "value" value $ toJsObject e


el_setOnMouseMove :: (MouseEvent -> Effect Unit) -> Element -> Effect Unit
el_setOnMouseMove eventHandlerF elem = setEventHandler "onmousemove" eventHandlerF (cast elem)


---------------------------------------------------------------
-- SVG
---------------------------------------------------------------

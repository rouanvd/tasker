module Webfu.Mithril.HTML
( module Webfu.Mithril.HTML
, module Webfu.Data.ObjMap
, module Webfu.DOM.Events
) where

import Prelude (Unit, unit, class Show, show, (<>), (#))
import Data.Either
import Data.Array (foldl)
import Effect (Effect)
import Data.Function.Uncurried (Fn2, runFn2)
import Foreign.Object (Object)
import Foreign.Object (empty, insert, fold) as StrMap
import Webfu.DOM.Events (MouseEvent, KeyboardEvent, preventDefault)
import Webfu.Data.ObjMap (Obj, empty, (:=), mkObjWithProps)
import Webfu.Mithril (Component, VNode, mkTextVNode, mkVNode, mkComponentVNode)


---------------------------------------------------------------
-- ATTRIBUTE BUILDER
---------------------------------------------------------------

type Attributes = Obj
type AttributeSetterF = Attributes -> Attributes
type AttributeSpec = Array AttributeSetterF


---------------------------------------------------------------
-- GENERIC STRING MAP BUILDER
---------------------------------------------------------------

type StrMapSetterF = Object String -> Object String
type StrMapSpec = Array StrMapSetterF

strMapInsert :: String -> String -> StrMapSetterF
strMapInsert = StrMap.insert

infixl 5 strMapInsert as :


---------------------------------------------------------------
-- CSS BUILDER
---------------------------------------------------------------

type CssRules = Object String
type CssRuleSetterF = CssRules -> CssRules
type CssRuleSpec = Array CssRuleSetterF


css :: CssRuleSpec -> String
css setterFs =
  foldl (\strMap setterF -> setterF strMap) StrMap.empty setterFs
  # StrMap.fold (\acc key val -> acc <> key <> ":" <> val <> ";") ""




---------------------------------------------------------------
-- MITHRIL COMPONENT
---------------------------------------------------------------

mcomp :: Component -> VNode
mcomp component = mkComponentVNode component


---------------------------------------------------------------
-- CONTENT
---------------------------------------------------------------

main :: AttributeSpec -> Array VNode -> VNode
main attrs childNodes = mkVNode (Left "main") (mkObjWithProps attrs) childNodes

h1 :: AttributeSpec -> Array VNode -> VNode
h1 attrs childNodes = mkVNode (Left "h1") (mkObjWithProps attrs) childNodes

h1' :: AttributeSpec -> String -> VNode
h1' attrs text = mkTextVNode (Left "h1") (mkObjWithProps attrs) text

br :: AttributeSpec -> VNode
br attrs = mkVNode (Left "br") (mkObjWithProps attrs) []

a :: AttributeSpec -> Array VNode -> VNode
a attrs childNodes = mkVNode (Left "a") (mkObjWithProps attrs) childNodes

a' :: AttributeSpec -> String -> VNode
a' attrs text = mkTextVNode (Left "a") (mkObjWithProps attrs) text

div :: AttributeSpec -> Array VNode -> VNode
div attrs childNodes = mkVNode (Left "div") (mkObjWithProps attrs) childNodes


---------------------------------------------------------------
-- TABULAR DATA
---------------------------------------------------------------

table :: AttributeSpec -> Array VNode -> VNode
table attrs childNodes = mkVNode (Left "table") (mkObjWithProps attrs) childNodes

thead :: AttributeSpec -> Array VNode -> VNode
thead attrs childNodes = mkVNode (Left "thead") (mkObjWithProps attrs) childNodes

tbody :: AttributeSpec -> Array VNode -> VNode
tbody attrs childNodes = mkVNode (Left "tbody") (mkObjWithProps attrs) childNodes

tr :: AttributeSpec -> Array VNode -> VNode
tr attrs childNodes = mkVNode (Left "tr") (mkObjWithProps attrs) childNodes

th :: AttributeSpec -> Array VNode -> VNode
th attrs childNodes = mkVNode (Left "th") (mkObjWithProps attrs) childNodes

th' :: AttributeSpec -> String -> VNode
th' attrs text = mkTextVNode (Left "th") (mkObjWithProps attrs) text

td :: AttributeSpec -> Array VNode -> VNode
td attrs childNodes = mkVNode (Left "td") (mkObjWithProps attrs) childNodes

td' :: AttributeSpec -> String -> VNode
td' attrs text = mkTextVNode (Left "td") (mkObjWithProps attrs) text



---------------------------------------------------------------
-- INPUT CONTROLS
---------------------------------------------------------------

button :: AttributeSpec -> Array VNode -> VNode
button attrs childNodes = mkVNode (Left "button") (mkObjWithProps attrs) childNodes

button' :: AttributeSpec -> String -> VNode
button' attrs text = mkTextVNode (Left "button") (mkObjWithProps attrs) text

select :: AttributeSpec -> Array VNode -> VNode
select attrs childNodes = mkVNode (Left "select") (mkObjWithProps attrs) childNodes

option :: AttributeSpec -> String -> VNode
option attrs text = mkTextVNode (Left "option") (mkObjWithProps attrs) text

input :: AttributeSpec -> VNode
input attrs = mkVNode (Left "input") (mkObjWithProps attrs) []



---------------------------------------------------------------
-- SVG
---------------------------------------------------------------

svg :: AttributeSpec -> Array VNode -> VNode
svg attrs childNodes = mkVNode (Left "svg") (mkObjWithProps (["xmlns" := "http://www.w3.org/2000/svg"] <> attrs)) childNodes

svgGroup :: AttributeSpec -> Array VNode -> VNode
svgGroup attrs childNodes = mkVNode (Left "g") (mkObjWithProps attrs) childNodes

svgText :: AttributeSpec -> String -> VNode
svgText attrs text = mkTextVNode (Left "text") (mkObjWithProps attrs) text

svgRect :: AttributeSpec -> VNode
svgRect attrs = mkVNode (Left "rect") (mkObjWithProps attrs) []

svgCircle :: AttributeSpec -> VNode
svgCircle attrs = mkVNode (Left "circle") (mkObjWithProps attrs) []

svgLine :: AttributeSpec -> VNode
svgLine attrs = mkVNode (Left "line") (mkObjWithProps attrs) []


---------------------------------------------------------------
-- KEYBOARD EVENTS
---------------------------------------------------------------

onKeyDown :: (KeyboardEvent -> Effect Unit) -> AttributeSetterF
onKeyDown f = "onkeydown" := (\e -> f e)

onKeyPress :: (KeyboardEvent -> Effect Unit) -> AttributeSetterF
onKeyPress f = "onkeypress" := (\e -> f e)

onKeyUp :: (KeyboardEvent -> Effect Unit) -> AttributeSetterF
onKeyUp f = "onkeyup" := (\e -> f e)


---------------------------------------------------------------
-- MOUSE EVENTS
---------------------------------------------------------------

onMouseEnter :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseEnter f = "onmouseenter" := (\e -> f e)

onMouseLeave :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseLeave f = "onmouseleave" := (\e -> f e)

onMouseOver :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseOver f = "onmouseover" := (\e -> f e)

onMouseOut :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseOut f = "onmouseout" := (\e -> f e)

onMouseMove :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseMove f = "onmousemove" := (\e -> f e)

onMouseDown :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseDown f = "onmousedown" := (\e -> f e)

onMouseUp :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onMouseUp f = "onmouseup" := (\e -> f e)

onClick :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onClick f = "onclick" := (\e -> f e)

onDblClick :: (MouseEvent -> Effect Unit) -> AttributeSetterF
onDblClick f = "ondblclick" := (\e -> f e)

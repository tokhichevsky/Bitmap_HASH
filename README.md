# Bitmap_HASH
Generates a hash from the image. Compares images using Hamming distance.

This is my first attempt to do something interesting using С#. The code is very rough.

Function: ResizeBitmap (private)
---------------
Creates a new bitmap with new scales from received bitmap.

Function: BitmapToByteArray (private)
---------------
Convert a bitmap into a byte array.

Function: BytesRGBtoBlackWhite (private)
---------------
Bitmap converte to a byte array is converted to black and white.
Visual demonstration of this complex operation:
![An image to understand the complexity of the function](https://github.com/tokhichevsky/Bitmap_HASH/blob/master/Color_Operation.png)

Function: AveragingSimplification (private)
---------------
Calculates the average of all the elements in an array. 
If the value of the array element is less than the average, the element is assigned the value 0, otherwise it is 1.

Function: GetHASH (public)
---------------
### Parameters:
#### mode:
* "low": New size is 4x4
* "normal": New size is 8x4
* "high": New size is 8x8

#### bitmode:
* 32: using BitConverter.ToUInt32
* 64: using BitConverter.ToUInt64


`Received Bitmap` → **ResizeBitmap** → **BitmapToByteArray** → **BytesRGBtoBlackWhite** → **AveragingSimplification** → **BitConverter** → `HASH`

Function: HammingDistance (public)
---------------
Calculates Hamming distance.

Read more: [Wikipedia: Hamming distance](https://en.wikipedia.org/wiki/Hamming_distance)

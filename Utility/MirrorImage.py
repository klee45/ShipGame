from PIL import Image, ImageDraw


def main(input_name, output_name):
    # load the image, create the mirrored image, and the result placeholder
    img = Image.open(input_name)
    mirror = img.transpose(Image.FLIP_LEFT_RIGHT).transpose(Image.ROTATE_90)
    sz = max(img.size + mirror.size)
    result = Image.new(img.mode, (sz, sz))
    result.paste(img, (0, 0) + img.size)

    # now paste the mirrored image, but with a triangular binary mask
    mask = Image.new('1', mirror.size)
    draw = ImageDraw.Draw(mask)
    draw.polygon([0, 0, 0, sz, sz, sz], outline='white', fill='white')
    result.paste(mirror, (0, 0) + mirror.size, mask)

    # clean up and save the result
    del mirror, mask, draw
    result.save(output_name)


if __name__ == "__main__":
    main()

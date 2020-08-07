using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace NiceET
{
	public static class ZipHelper
	{
		public static byte[] Compress(byte[] content)
		{
			//return content;
			Deflater compressor = new Deflater();
			compressor.SetLevel(Deflater.BEST_COMPRESSION);

			compressor.SetInput(content);
			compressor.Finish();

			using (MemoryStream bos = new MemoryStream(content.Length))
			{
				var buf = new byte[1024];
				while (!compressor.IsFinished)
				{
					int n = compressor.Deflate(buf);
					bos.Write(buf, 0, n);
				}
				return bos.ToArray();
			}
		}

		public static byte[] Decompress(byte[] content)
		{
			return Decompress(content, 0, content.Length);
		}

		public static byte[] Decompress(byte[] content, int offset, int count)
		{
			//return content;
			Inflater decompressor = new Inflater();
			decompressor.SetInput(content, offset, count);

			using (MemoryStream bos = new MemoryStream(content.Length))
			{
				var buf = new byte[1024];
				while (!decompressor.IsFinished)
				{
					int n = decompressor.Inflate(buf);
					bos.Write(buf, 0, n);
				}
				return bos.ToArray();
			}
		}
	}
}


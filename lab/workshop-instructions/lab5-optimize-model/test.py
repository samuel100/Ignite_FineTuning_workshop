import torch

devices = ["cpu", "cuda"]
dtypes = [torch.float32, torch.float16, torch.bfloat16]

for device in devices:
    for dtype in dtypes:
        print(f"device: {device}, dtype: {dtype}")
        try:
            torch.matmul(torch.randn(2, 3, device=device, dtype=dtype), torch.randn(3, 2, device=device, dtype=dtype))
            print("Success")
        except Exception as e:
            print(e)
        print()
import onnxruntime_genai as og
import argparse
import time

def main(args):
    if args.verbose: print("Loading model...")
    if args.timings:
        started_timestamp = 0
        first_token_timestamp = 0

    model = og.Model(f'{args.model}')
    if args.verbose: print("Model loaded")
    tokenizer = og.Tokenizer(model)
    tokenizer_stream = tokenizer.create_stream()
    if args.verbose: print("Tokenizer created")
    if args.verbose: print()

    if hasattr(args, "adapter"):
        adapters = og.Adapters(model)
        adapters.load(f'{args.adapter}', "travel")
        if args.verbose: print("Loaded adapter")

    search_options = {name:getattr(args, name) for name in ['do_sample', 'max_length', 'min_length', 'top_p', 'top_k', 'temperature', 'repetition_penalty'] if name in args}
    search_options["past_present_share_buffer"] = False

    if args.verbose: print(search_options)
    
    if args.chat_template:
        if args.chat_template.count('{') != 1 or args.chat_template.count('}') != 1:
            print("Error, chat template must have exactly one pair of curly braces, e.g. '<|user|>\n{input} <|end|>\n<|assistant|>'")
            exit(1)

    # Keep asking for input prompts in a loop
    while True:
        text = input("Input: ")
        if not text:
            print("Error, input cannot be empty")
            continue

        if args.timings: started_timestamp = time.time()

        # If there is a chat template, use it otherwise default to phi format
        prompt = f"<|user|>\n{text}<|end|>\n<|assistant|>\n"
        if args.chat_template:
            prompt = f'{args.chat_template.format(input=text)}'

        input_tokens = tokenizer.encode(prompt)

        params = og.GeneratorParams(model)
        params.set_search_options(**search_options)
        params.input_ids = input_tokens
        generator = og.Generator(model, params)
        if args.verbose: print("Generator created")

        if hasattr(args, "adapter"):
            generator.set_active_adapter(adapters, "travel")
            if args.verbose: print("Set active adapter")

        if args.verbose: print("Running generation loop ...")
        if args.timings:
            first = True
            new_tokens = []

        print()
        print("Output: ", end='', flush=True)

        try:
            while not generator.is_done():
                generator.compute_logits()
                generator.generate_next_token()
                if args.timings:
                    if first:
                        first_token_timestamp = time.time()
                        first = False

                new_token = generator.get_next_tokens()[0]
                print(tokenizer_stream.decode(new_token), end='', flush=True)
                if args.timings: new_tokens.append(new_token)
        except KeyboardInterrupt:
            print("  --control+c pressed, aborting generation--")
        print()
        print()

        # Delete the generator to free the captured graph for the next generator, if graph capture is enabled
        del generator

        if args.timings:
            prompt_time = first_token_timestamp - started_timestamp
            run_time = time.time() - first_token_timestamp
            print(f"Prompt length: {len(input_tokens)}, New tokens: {len(new_tokens)}, Time to first: {(prompt_time):.2f}s, Prompt tokens per second: {len(input_tokens)/prompt_time:.2f} tps, New tokens per second: {len(new_tokens)/run_time:.2f} tps")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(argument_default=argparse.SUPPRESS, description="End-to-end AI Question/Answer example for gen-ai")
    parser.add_argument('-m', '--model', type=str, required=True, help='Onnx model folder path (must contain config.json and model.onnx)')
    parser.add_argument('-a', '--adapter', type=str, help='Onnx adapter path')
    parser.add_argument('-i', '--min_length', type=int, help='Min number of tokens to generate including the prompt')
    parser.add_argument('-l', '--max_length', type=int, help='Max number of tokens to generate including the prompt')
    parser.add_argument('-ds', '--do_random_sampling', action='store_true', help='Do random sampling. When false, greedy or beam search are used to generate the output. Defaults to false')
    parser.add_argument('-p', '--top_p', type=float, help='Top p probability to sample with')
    parser.add_argument('-k', '--top_k', type=int, help='Top k tokens to sample from')
    parser.add_argument('-t', '--temperature', type=float, help='Temperature to sample with')
    parser.add_argument('-r', '--repetition_penalty', type=float, help='Repetition penalty to sample with')
    parser.add_argument('-v', '--verbose', action='store_true', default=False, help='Print verbose output and timing information. Defaults to false')
    parser.add_argument('-g', '--timings', action='store_true', default=False, help='Print timing information for each generation step. Defaults to false')
    parser.add_argument('-c', '--chat_template', type=str, default='', help='Chat template to use for the prompt. User input will be injected into {input}')
    args = parser.parse_args()
    main(args)
